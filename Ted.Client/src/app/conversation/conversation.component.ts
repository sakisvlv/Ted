import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConversationDataService } from './conversation-data.service';
import { ToastrService } from 'ngx-toastr';
import { LoaderService } from '../core/loader/loader.service';
import { Message, Conversation } from './conversation.models';
import { AuthService } from '../core/auth/services/auth.service';


@Component({
  selector: 'app-conversation',
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.scss']
})
export class ConversationComponent implements OnInit, AfterViewChecked {

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
    private route: ActivatedRoute
  ) { }

  ngOnInit() {

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
    console.log("got");

    this.page++;
    this.getMessages();
  }

  onSubmit(event) {
    if (event.key == "Enter") {
      if (this.messageToSend == "") {
        return;
      }
      this.sendMessage();
    }

  }

  sendMessage() {
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
