import * as $ from 'jquery';
import { Section } from './Section';
import { FlowSection } from './FlowSection';
import { FlowData } from './Model';

export class MessageSection extends FlowSection
{
    private _linkClickHandler: () => void;

    constructor()
    {
        super($('#message-section'));
        this._linkClickHandler = () => { };
    }

    install(): void
    {
        $('#message-link').click(evt =>
        {
            evt.preventDefault();
            this._linkClickHandler();
        });
    }

    activate(input: FlowData, spinner: () => void): Promise<FlowData>
    {
        if (!input.message)
            // We can't do anything if there's no message.    
            return Promise.resolve(input);
        
        // Set the message.
        $('#message-pre-link').text(input.message.preLink);
        $('#message-link').text(input.message.link);
        $('#message-post-link').text(input.message.postLink);
        
        return new Promise(resolve => this._linkClickHandler = () =>
        {
            resolve({ rsvp: input.rsvp });
        });
    }
}
