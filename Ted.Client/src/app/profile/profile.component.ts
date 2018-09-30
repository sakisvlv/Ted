import { Component, OnInit } from '@angular/core';
import { Experience, Education, PersonalSkill, Skills } from "./profile.models";
import { ProfileDataService } from './profile-data.service';
import { LoaderService } from '../core/loader/loader.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  experienceModalState = 'closed';
  educationModalState = 'closed';
  personalSkillsModalState = 'closed';

  experience: Experience;
  education: Education;
  personalSkill: PersonalSkill;

  experiences: Experience[] = [];
  educations: Education[] = [];
  personalSkills: PersonalSkill[] = [];

  skills: Skills = new Skills();

  startDateTime: Date = new Date(Date.now());

  constructor(
    private profileDataService: ProfileDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.onInit();
  }

  onInit() {
    this.init();
    this.loaderService.show();
    this.profileDataService.getUserSkills().subscribe(
      result => {
        this.educations = result.Educations;
        this.educations.forEach(x => x.StartDate = new Date(x.StartDate));
        this.educations.forEach(x => x.EndDate = new Date(x.EndDate));

        this.experiences = result.Experiences;
        this.experiences.forEach(x => x.StartDate = new Date(x.StartDate));
        this.experiences.forEach(x => x.EndDate = new Date(x.EndDate));

        this.personalSkills = result.PersonalSkills;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  init() {
    this.experience = new Experience();
    this.experience.StartDate = new Date(Date.now());
    this.experience.EndDate = new Date(Date.now());
    this.experience.StillThere = false;

    this.education = new Education();
    this.education.StartDate = new Date(Date.now());
    this.education.EndDate = new Date(Date.now());
    this.education.StillThere = false;

    this.personalSkill = new PersonalSkill();
  }


  openExperienceModal(experience: Experience) {
    if (experience == null) {
      this.init();
    }
    else {
      this.experience = experience;
    }
    this.experienceModalState = 'opened';
  }

  openEducationModalModal(education: Education) {
    if (education == null) {
      this.init();
    }
    else {
      this.education = education;

    }
    this.educationModalState = 'opened';
  }

  openPersonalSkillsModal(personalSkill: PersonalSkill) {
    if (personalSkill == null) {
      this.init();
    }
    else {
      this.personalSkill = personalSkill;
    }
    this.personalSkillsModalState = 'opened';
  }

  saveExperience() {
    this.profileDataService.saveExpiriance(this.experience).subscribe(
      result => {
        result.StartDate = new Date(Date.now());
        result.EndDate = new Date(Date.now());
        if (this.experience.Id != undefined) {
          this.experiences.splice(this.experiences.indexOf(this.experiences.find(x => x.Id == this.experience.Id)[0]), 1);
        }
        this.experiences.push(result);
        this.experienceModalState = 'closed';
        this.loaderService.hide();
        this.onInit();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  saveEducations() {
    this.profileDataService.saveEducation(this.education).subscribe(
      result => {
        result.StartDate = new Date(Date.now());
        result.EndDate = new Date(Date.now());
        if (this.experience.Id != undefined) {
          this.educations.splice(this.educations.indexOf(this.educations.find(x => x.Id == this.education.Id)[0]), 1);
        }
        this.educations.push(result);
        this.educationModalState = 'closed';
        this.onInit();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  savePersonalSkills() {
    this.profileDataService.savePersonalSkill(this.personalSkill).subscribe(
      result => {
        if (this.personalSkill.Id != undefined) {
          this.personalSkills.splice(this.personalSkills.indexOf(this.personalSkills.find(x => x.Id == this.personalSkill.Id)[0]), 1);
        }
        this.personalSkills.push(result);
        this.personalSkillsModalState = 'closed';
        this.onInit();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  deleteSkill(id: string, type: string) {
    this.profileDataService.deleteSkill(id, type).subscribe(
      result => {
        switch (type) {
          case "experience":
            this.experiences.splice(this.experiences.indexOf(this.experiences.find(x => x.Id == id)[0]), 1);
            this.experienceModalState = 'closed';
            break;
          case "education":
            this.educations.splice(this.educations.indexOf(this.educations.find(x => x.Id == id)[0]), 1);
            this.educationModalState = 'closed';
            break;
          case "personalSkill":
            this.personalSkills.splice(this.personalSkills.indexOf(this.personalSkills.find(x => x.Id == id)[0]), 1);
            this.personalSkillsModalState = 'closed';
            break;
        }
        this.loaderService.hide();
      },
      error => {
        this.experienceModalState = 'closed';
        this.educationModalState = 'closed';
        this.personalSkillsModalState = 'closed';
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

}
