export class Experience {
    Id: string;
    Title: string;
    Description: string;
    IsPublic: boolean;
    Company: string;
    StartDate: Date;
    EndDate: Date;
    StillThere: boolean;
    Link: string;
}

export class Post {
    Id: string;
    Title: string;
    User: UserSmall;
    IsPublic: boolean;
    Subscribers: UserSmall[];
    Comments: Comment[];
    Content: any;
    PostedDate: Date;
}

export class UserSmall {
    Id: string;
    FirstName: string;
    LastName: string;
    CurrentState: string;
}

export class Comment {
    Id: string;
    UserId: string;
    Text: string;
}

export enum PostType {
    Image,
    Video,
    Article,
    Audio
}