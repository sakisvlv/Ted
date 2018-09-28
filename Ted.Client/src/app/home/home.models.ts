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
    FileName: string;
    FileUrl: string;
    PostedDate: Date;
    Type: PostType;
    Description: string;
}

export class UserSmall {
    Id: string;
    FirstName: string;
    LastName: string;
    CurrentState: string;
    IsFriend: boolean;
}

export class Comment {
    Id: string;
    User: UserSmall;
    Text: string;
    CommentDate: Date
}

export enum PostType {
    Image,
    Video,
    Article,
    Audio
}

export class PostMetaData {
    PostId: string;
    Title: string;
}