// A day in January when people might like to travel to/from Adelaide
export type TravelDay = '18' | '19' | '20' | '21' | '22' | '23';

export type Offer = { [K in TravelDay]: number };

// All the information from the survey section, or from the database
export interface Survey
{
    adults: number;
    driving: boolean | null;
    busFrom: boolean;
    busTo: boolean;
    offer: Offer;
    moochFrom: TravelDay | null;
    moochTo: TravelDay | null;
    wineTour: boolean;
    dietary: string;
    comments: string;
}    

// The server sends us data, and we send the server data, with this structure.
export interface Rsvp
{
    authToken?: string;
    survey?: Survey;
}

// A message to display to the user.
export interface Message
{
    preLink: string;
    link: string;
    postLink: string;
}

// This is the type of data which is piped between sections in the application flow.
export interface FlowData
{
    rsvp: Rsvp;
    message?: Message;
}
