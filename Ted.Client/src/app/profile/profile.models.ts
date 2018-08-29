class Skill {
    Id: string;
    Title: string;
    Description: string;
    IsPublic: boolean;
}

export class PersonalSkill extends Skill {
}

export class Experience extends Skill {
    Company: string;
    StartDate: Date;
    EndDate: Date;
    StillThere: boolean;
    Link: string;
}

export class Education extends Skill {
    Degree: string;
    Field: string;
    Grade: number;
    StartDate: Date;
    EndDate: Date;
    StillThere: boolean;
    Link: string;
}

export class Skills {
    Experiences: Experience[];
    Educations: Education[];
    PersonalSkills: PersonalSkill[];
}