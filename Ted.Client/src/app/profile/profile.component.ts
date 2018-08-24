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

  experiences = [];
  educations = [];
  personalSkills = [];

  skills: Skills = new Skills();

  startDateTime: Date = new Date(Date.now());

  constructor(
    private profileDataService: ProfileDataService,
    private loaderService: LoaderService,
    private toastrService: ToastrService
  ) { }

  ngOnInit() {
    this.init();
    this.profileDataService.getUserSkills().subscribe(
      result => {
        this.educations = result.Educations;
        this.experiences = result.Experiences;
        this.personalSkills = result.PersonalSkills;
        console.log(result);
        this.loaderService.hide();
      },
      error => {
        console.log(error);
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


  openExperienceModal(index: number) {
    if (index == -1) {
      this.init();
    }
    this.experienceModalState = 'opened';
  }

  openEducationModalModal(index: number) {
    if (index == -1) {
      this.init();
    }
    this.educationModalState = 'opened';
  }

  openPersonalSkillsModal(index: number) {
    if (index == -1) {
      this.init();
    }
    this.personalSkillsModalState = 'opened';
  }

}
