import * as $ from 'jquery';

// A tab-like panel of which only one at a time is shown
export class Section
{
    protected constructor(protected sectionJq: JQuery) { }

    // Overridden by subclasses to set up event handlers etc.
    install(): void { }
    
    // Show this section, having first hidden any other visible section.
    show(): void
    {
        this.hideOthers();
        this.sectionJq.toggleClass('foreground', true);
    }

    private hideOthers(): void
    {
        $('.section.foreground').not(this.sectionJq).toggleClass('foreground', false);
    }
}
