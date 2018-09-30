import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@aspnet/signalr';

import { ConversationDataService } from './conversation-data.service';
import { ToastrService } from 'ngx-toastr';
import { LoaderService } from '../core/loader/loader.service';
import { AuthService } from '../core/auth/services/auth.service';

import { Message, Conversation } from './conversation.models';
import { environment } from '../../environments/environment';
import { BudgiesService } from '../core/navbar/budgies.service';



@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnInit, AfterViewChecked {

  private signalR: string = environment.signalR + "messages";
  private hubConnection: HubConnection;

  @ViewChild('scrollMe') private myScrollContainer: ElementRef;

  messages: Message[] = [];
  convaersations: Conversation[];
  selectedConversation: Conversation;
  messageToSend = "";
  page = 0;
  userId: string;

  constructor(
    private conversationDataService: ConversationDataService,
    private toastrService: ToastrService,
    private loaderService: LoaderService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private budgiesService: BudgiesService
  ) { }

  ngOnInit() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.signalR, { accessTokenFactory: () => { return this.authService.getToken() } })
      .build();
    this.hubConnection.start().catch(err => console.error(err.toString()));

    this.hubConnection.on('ReceiveMessage', (message: string, conversationId: string) => {
      if (this.selectedConversation.Id == conversationId) {
        let mes = new Message();
        mes.Sender = this.messages.find(x => x.Sender.Id != this.authService.userId).Sender;
        mes.DateSended = new Date(Date.now());
        mes.Text = message;
        this.messages.push(mes);
      }
      else {
        let con = this.convaersations.find(x => x.Id == conversationId);
        con.HasNewMessagees = true;
      }
    });

    this.route.params.subscribe(params => {
      this.userId = params['id'];

      this.conversationDataService.getConversations().subscribe(
        result => {
          this.convaersations = result;

          if (this.userId) {
            let flag = false;
            for (let i = 0; i < this.convaersations.length; i++) {
              if (this.convaersations[i].ToUser.Id == this.userId) {
                flag = true;
                this.selectedConversation = this.convaersations[i];
                break;
              }
            }
            if (flag == true) {
              this.getMessages();
              return;
            }
            else {
              this.conversationDataService.startConversation(this.userId).subscribe(
                res => {
                  this.convaersations.push(res);
                  this.selectedConversation = res;
                  this.getMessages();
                  this.loaderService.hide();
                },
                error => {
                  this.loaderService.hide();
                  this.toastrService.error(error.error, 'Error');
                }
              );
            }
          }
          if (result.length != 0) {
            this.selectedConversation = result[0];
            this.getMessages();
          }

          this.loaderService.hide();
        },
        error => {
          this.loaderService.hide();
          this.toastrService.error(error.error, 'Error');
        }
      );


    },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );


  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }


  getMessages() {
    this.conversationDataService.ackConversation(this.selectedConversation.Id).subscribe(
      result => {
        this.selectedConversation.HasNewMessagees = false;
        this.loaderService.hide();
        this.budgiesService.getBudgies();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
    this.conversationDataService.getMessages(this.selectedConversation.Id, this.page).subscribe(
      result => {
        this.messages = result.concat(this.messages);
        this.scrollToBottom();
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  changeConversation(conversation: Conversation) {
    this.selectedConversation = conversation;
    this.messages = [];
    this.page = 0;
    this.getMessages();
  }

  showMore() {
    this.page++;
    this.getMessages();
  }

  onSubmit(event) {
    if (event.key == "Enter") {
      if (this.messageToSend.trim() == "") {
        return;
      }
      this.sendMessage();
    }

  }

  sendMessage() {
    if (this.messageToSend.trim() == "") {
      return;
    }
    this.conversationDataService.sendMessage(this.messageToSend, this.selectedConversation.Id).subscribe(
      result => {
        this.messages.push(result);
        console.log(result);
        this.messageToSend = "";
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  scrollToBottom(): void {
    try {
      this.myScrollContainer.nativeElement.scrollTop = this.myScrollContainer.nativeElement.scrollHeight;
    } catch (err) { }
  }



}
