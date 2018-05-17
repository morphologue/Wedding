import * as $ from 'jquery';
import { Section } from './Section';
import { FlowSection } from './FlowSection';
import { FlowData, Survey, TravelDay, Offer, Rsvp } from './Model';
import { handleAjaxError, makeServerUrl } from './Util';

export class SurveySection extends FlowSection
{
    private _submitHandler: () => void;
    private _paxJq: JQuery;
    private _drivingYes: JQuery;
    private _drivingNo: JQuery;
    
    constructor()
    {
        super($('#survey-section'));
        this._paxJq = $('#input-pax');
        this._drivingYes = $('#input-driving-yes');
        this._drivingNo = $('#input-driving-no');
    }

    install(): void
    {
        this.sectionJq.find('form').submit(evt =>
        {
            evt.preventDefault();
            this._submitHandler();
        });
        
        [this._paxJq, this._drivingYes, this._drivingNo].forEach(jq => jq.change(() => this._updateElementProperties()));
    }

    activate(input: FlowData, spinner: () => void): Promise<FlowData>
    {
        if (input.rsvp.survey)
            this._deobjectify(input.rsvp.survey);
        else
            this._resetForm();
        this._updateElementProperties();

        return new Promise(resolve => this._submitHandler = () =>
        {
            if (this._getPaxCount() && !this.sectionJq.find('input[type=radio]:checked').length) {
                alert('Please answer "Yes" or "No" to whether you are driving.');
                $('#submit-survey').blur();
                return;
            }

            const rsvp: Rsvp = {
                authToken: input.rsvp.authToken,
                survey: this._objectify()
            };

            spinner();
            $.ajax({
                url: makeServerUrl('Rsvp'),
                method: 'POST',
                contentType: 'application/json; charset=UTF-8',
                data: JSON.stringify(rsvp),
                success: (response: Rsvp) => resolve({
                    rsvp: response,
                    message: {
                        preLink: 'Thank you. Your response has been recorded, but you can still ',
                        link: 'edit',
                        postLink: ' it if you need to.'
                    }
                }),
                error: (xhr, err1, err2) => handleAjaxError(xhr, err2 || err1, { rsvp }, resolve, false)
            });
        })
    }

    // Reset all fields to their initial (blank) values.
    private _resetForm()
    {
        $('#input-pax').val('1');
        this.sectionJq.find('input[type=radio]:checked, input[type=checkbox]:checked').prop('checked', false);
        this.sectionJq.find('input[type=text], textarea').val('');
        this.sectionJq.find('select:not(#input-pax)').val('0');
    }

    // Set the current control values from a Survey object.
    private _deobjectify(survey: Survey): void
    {
        $('#input-pax').val(survey.adults);
        $('#input-driving-yes').prop('checked', survey.driving);
        $('#input-driving-no').prop('checked', survey.driving === false);
        $('#input-busfrom').prop('checked', survey.busFrom);
        $('#input-busto').prop('checked', survey.busTo);
        this.sectionJq.find('.input-offerx').each((idx, el) =>
        {
            const el_jq = $(el);
            el_jq.val(survey.offer[(el_jq.attr('data-x') + '') as TravelDay]);
        });
        $('#input-moochfrom').val(survey.moochFrom || '0');
        $('#input-moochto').val(survey.moochTo || '0');
        $('#input-wine').prop('checked', survey.wineTour);
        $('#input-dietary').val(survey.dietary);
        $('#input-comments').val(survey.comments);
    }

    // Scrape the current control values into a Survey object.
    private _objectify(): Survey
    {
        const moochfrom_val = $('#input-moochfrom').val() + '';
        const moochto_val = $('#input-moochto').val() + '';
        return {
            adults: parseInt($('#input-pax').val() + ''),
            driving: $('#input-driving-yes').prop('checked') ? true : ($('#input-driving-no').prop('checked') ? false : null),
            busFrom: !!$('#input-busfrom').prop('checked'),
            busTo: !!$('#input-busto').prop('checked'),
            offer: (() =>
            {
                let builder: Offer = {} as Offer;
                this.sectionJq.find('.input-offerx').each((idx, el) =>
                {
                    const el_jq = $(el);
                    builder[(el_jq.attr('data-x') + '') as TravelDay] = parseInt(el_jq.val() + '');
                });
                return builder;
            })(),
            moochFrom: moochfrom_val === '0' ? null : moochfrom_val as TravelDay,
            moochTo: moochto_val === '0' ? null : moochto_val as TravelDay,
            wineTour: !!$('#input-wine').prop('checked'),
            dietary: $('#input-dietary').val() + '',
            comments: $('#input-comments').val() + ''
        };
    }
    
    // Set dynamic properties (e.g. visibility) according to the current state of the controls.
    private _updateElementProperties(): void
    {
        const pax = this._getPaxCount();
        this.sectionJq.find('.for-attendees').toggle(pax > 0);
        if (pax > 0) {
            this.sectionJq.find('.singular').toggle(pax === 1);
            this.sectionJq.find('.plural').toggle(pax !== 1);
            this.sectionJq.find('.for-drivers').toggle(this._drivingYes.is(':checked'));
            this.sectionJq.find('.for-non-drivers').toggle(this._drivingNo.is(':checked'));
        }    
    }

    private _getPaxCount(): number
    {
        // Handle the fact that val() can return string, number or undefined.
        return parseInt(this._paxJq.val() + '');
    }
}
