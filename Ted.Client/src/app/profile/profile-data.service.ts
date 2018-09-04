import { Injectable } from '@angular/core';

import { Skills, Experience, Education, PersonalSkill } from './profile.models';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfileDataService {

  private apiUrl: string = environment.apiUri + "User/";

  constructor(private http: HttpClient) { }

  getUserSkills() {
    return this.http.get<Skills>(this.apiUrl + "Skills");
  }

  saveExpiriance(experience: Experience) {
    return this.http.put<Experience>(this.apiUrl + "SaveExperience", experience);
  }

  saveEducation(education: Education) {
    return this.http.put<Education>(this.apiUrl + "SaveEducation", education);
  }

  savePersonalSkill(experience: PersonalSkill) {
    return this.http.put<PersonalSkill>(this.apiUrl + "SavePersonalSkill", PersonalSkill);
  }

}
