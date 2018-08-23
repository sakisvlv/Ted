import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  experienceModalState = 'closed';
  educationModalState = 'closed';
  personalSkillsModalState = 'closed';

  startDateTime: Date = new Date(Date.now());

  constructor() { }

  ngOnInit() {
  }


  openExperienceModal(index: string) {
    this.experienceModalState = 'opened';
  }

  openEducationModalModal(index: string) {
    this.educationModalState = 'opened';
  }

  openPersonalSkillsModal(index: string) {
    this.personalSkillsModalState = 'opened';
  }

}
