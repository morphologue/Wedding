import * as $ from 'jquery';
import { Section } from './Section';

export class SpinnerSection extends Section
{
    constructor()
    {
        super($('#spinner-section'));
    }
}
