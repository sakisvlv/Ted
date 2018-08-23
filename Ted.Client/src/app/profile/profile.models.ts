class Skill {
    Id: string;
    Title: string;
    Description: string;
    IsPublic: boolean;
}

class PersonalSkill extends Skill {
}

class Experience extends Skill {
    Company: string;
    StartDate: Date;
    EndDate: Date;
    Link: string;
}

class Education extends Skill {
    Degree: string;
    Feild: string;
    Grade: string;
    StartDate: Date;
    EndDate: Date;
    Link: string;
}