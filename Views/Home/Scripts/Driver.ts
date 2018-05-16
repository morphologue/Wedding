import { Section } from './Section';
import { SpinnerSection } from './SpinnerSection';
import { AuthSection } from './AuthSection';
import { SurveySection } from './SurveySection';
import { MessageSection } from './MessageSection';
import { FlowData, Rsvp, Message } from './Model';
import { FlowSection } from './FlowSection';

// Direct the overall flow of the application.
export class Driver
{
    private _messageSection: MessageSection;
    private _flow: FlowSection[];
    private _spinnerSection: SpinnerSection;
    private _allSections: Section[];
    
    constructor()
    {
        this._flow = [
            new AuthSection(),
            new SurveySection(),
            this._messageSection = new MessageSection()
        ];
        this._allSections = [
            ...this._flow,
            // _spinnerSection stands outside the flow as it is between steps.
            this._spinnerSection = new SpinnerSection()
        ]
    }

    // Direct the high-level flow of the application.
    async drive(): Promise<never>
    {
        // Install the sections.
        this._allSections.forEach(c => c.install());

        let flow_idx = 0;
        let pipe: FlowData = { rsvp: {} };
        while (true) {
            // Make the current section visible and active.
            let active = this._flow[flow_idx];
            active.show();
            try {
                pipe = await active.activate(pipe, () => this._spinnerSection.show());
            } catch (e) {
                pipe = {
                    ...pipe,
                    message: {
                        preLink: `An unexpected error has occurred${typeof e === 'string' ? ': ' + e : ''}. Please `,
                        link: 'try again',
                        postLink: '.'
                    }
                };
            }    

            // Advance the flow.
            if (++flow_idx == this._flow.length)
                flow_idx = 0;
            if (pipe.message && this._flow[flow_idx] !== this._messageSection)
                flow_idx = this._flow.findIndex(c => c === this._messageSection);
        }
    }
}
