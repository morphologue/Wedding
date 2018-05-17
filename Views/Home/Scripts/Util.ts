import { FlowData, Survey } from "./Model";
import * as $ from "jquery";

export function makeServerUrl(path: string)
{
    return $('#hidden-url-prefix').val() + '/' + path;
}

export function handleAjaxError(xhr: JQueryXHR, msg: string, input: FlowData, resolve: (fd: FlowData) => void, logging_in: boolean)
{
    if (xhr.status === 401)
        // Unauthorized - assume session expiry or login failure
        resolve({
            rsvp: {
                // Strip authToken if present, as the user will need to log in again.
                survey: input.rsvp.survey
            },
            message: logging_in
                ? {
                    preLink: 'The information you entered did not match an invitee. Please ',
                    link: 'try again',
                    postLink: '.'
                }
                : {
                    preLink: 'Your session has expired. Please ',
                    link: 'log in',
                    postLink: ' again. Any changes you have made will be preserved.'
                }
        });
    else
        resolve({
            rsvp: input.rsvp,
            message: {
                preLink: `An unexpected error has occurred while contacting the server${msg ? ': ' + msg : ''}. Please `,
                link: 'try again',
                postLink: '.'
            }
        });
}
