import { Injectable } from '@angular/core';

import { environment } from '../../environments/environment'
import { HttpClient } from '@angular/common/http';
import { UserSmall } from '../home/home.models';
import { Conversation, Message } from './conversation.models';

@Injectable({
  providedIn: 'root'
})
export class ConversationDataService {

  private apiUrl: string = environment.apiUri + "Conversation/";

  constructor(private http: HttpClient) { }

  getConversations() {
    return this.http.get<Conversation[]>(this.apiUrl + "GetConversations");
  }

  getMessages(conversationId: string, page: number) {
    return this.http.get<Message[]>(this.apiUrl + "GetMessages/" + conversationId + "/" + page);
  }

  sendMessage(text: string, conversationId: string) {
    return this.http.post<Message>(this.apiUrl + "SendMessage", [text, conversationId]);
  }

  startConversation(userId: string) {
    return this.http.post<Conversation>(this.apiUrl + "StartConversation", [userId]);
  }

  ackConversation(conversationId: string) {
    return this.http.get<boolean>(this.apiUrl + "AckConversation/"+ conversationId);
  }

}
