import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';
import { UserSmall } from '../home/home.models';
import { Conversation } from './conversation.models';

@Injectable({
  providedIn: 'root'
})
export class ConversationDataService {

  private apiUrl: string = environment.apiUri + "Conversation/";

  constructor(private http: HttpClient) { }

  getConversations() {
    return this.http.get<Conversation[]>(this.apiUrl + "GetConversations");
  }
}
