import { UserSmall } from "../notifications/notifications.models";

export class Ad {
    Id: string;
    Title: string;
    Description: string;
    Company: string;
    Owner: UserSmall;
    Applicants : UserSmall[];
}