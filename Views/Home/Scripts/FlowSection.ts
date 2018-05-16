import { FlowData } from './Model';
import { Section } from './Section';

// A section which is part of the main application flow. It can receive data from and send data to
// other sections.
export class FlowSection extends Section
{
    protected constructor(jq: JQuery)
    {
        super(jq);
    }
    
    // Do the work of the section, optionally accepting data from another section and resolving with
    // data for the next section. Between the end of user interaction and the completion of the work
    // of the section, the spinner callback can be used to freeze the interface.
    activate(input: FlowData, spinner: () => void): Promise<FlowData>
    {
        return Promise.resolve(input);
    }
}
