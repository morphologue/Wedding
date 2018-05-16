import * as $ from 'jquery';
import { Section } from './Section';
import { FlowSection } from './FlowSection';
import { FlowData, Rsvp } from './Model';
import { handleAjaxError, makeServerUrl } from './Util';

export class AuthSection extends FlowSection
{
    private _submitHandler: () => void;

    constructor()
    {
        super($('#auth-section'));
    }

    install(): void
    {
        this.sectionJq.find('form').submit(evt =>
        {
            evt.preventDefault();
            this._submitHandler();
        });
    }

    activate(input: FlowData, spinner: () => void): Promise<FlowData>
    {
        return new Promise((resolve, reject) => {
            if(input.rsvp.authToken)
            {
                if (input.rsvp.survey) {
                    // We've already been given data and auth, so there's no need for this section.
                    resolve(input);
                    return;
                }

                // Pull the RSVP from the server without logging in again.
                spinner();
                $.ajax({
                    url: makeServerUrl('Rsvp'),
                    method: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    data: JSON.stringify({ authToken: input.rsvp.authToken }),
                    success: (response: Rsvp) => resolve({ rsvp: response }),
                    error: (xhr, msg) => handleAjaxError(xhr, msg, input, resolve, false)
                });
                return;
            }

            // If we're here, we can't escape logging in.
            this._resetForm();
            this._submitHandler = () =>
            {
                let scraped = {
                    name: ($('#input-name').val() || '') + '',
                    code: parseInt(($('#input-code').val() || '0') + '')
                };
                if (!scraped.name.trim() || scraped.code <= 0) {
                    alert('Please provide both your name and your invitation code.');
                    $('#submit-auth').blur();
                    return;
                }

                spinner();
                $.ajax({
                    url: makeServerUrl('Login'),
                    method: 'POST',
                    contentType: 'application/json; charset=UTF-8',
                    data: JSON.stringify(scraped),
                    success: (response: Rsvp) =>
                    {
                        resolve({
                            rsvp: {
                                authToken: response.authToken,
                                survey: input.rsvp.survey || response.survey
                            }
                        });
                    },
                    error: (xhr, msg) => handleAjaxError(xhr, msg, input, resolve, true)
                });
            };
        });
    }

    private _resetForm()
    {
        this.sectionJq.find('input[type=text]').val('');
    }
}
