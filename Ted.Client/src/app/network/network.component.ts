import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserSmall } from '../home/home.models';

import { NetworkDataService } from './network-data.service';
import { ToastrService } from 'ngx-toastr';
import { LoaderService } from '../core/loader/loader.service';
import { BudgiesService } from '../core/navbar/budgies.service';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss']
})
export class NetworkComponent implements OnInit {

  allFriends: UserSmall[][] = [];
  pendingFriends: UserSmall[][] = [];
  searchFriends: UserSmall[][];
  pending: number;
  found: number;
  all: number;
  searchString: string;

  constructor(
    private networkDataService: NetworkDataService,
    private toastrService: ToastrService,
    private loaderService: LoaderService,
    private router: Router,
    private budgiesService: BudgiesService
  ) { }

  ngOnInit() {
    this.networkDataService.getFriends().subscribe(
      result => {
        this.all = result.length;
        let current = 0;
        let fives = 0;
        for (let i = 0; i < result.length; i++) {
          if (current == 0) {
            this.allFriends[fives] = [];
          }
          this.allFriends[fives][current] = result[i];
          current++;
          if (current == 5) {
            current = 0;
            fives++;
          }
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );

    this.networkDataService.getPendingFriends().subscribe(
      result => {
        this.pending = result.length;
        let current = 0;
        let fives = 0;
        for (let i = 0; i < result.length; i++) {
          if (current == 0) {
            this.pendingFriends[fives] = [];
          }
          this.pendingFriends[fives][current] = result[i];
          current++;
          if (current == 5) {
            current = 0;
            fives++;
          }
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );

  }

  navigateToView(id: string) {
    this.router.navigate(['/view', { id: id }]);
  }

  search() {
    this.searchFriends = [];
    this.networkDataService.searchFriends(this.searchString).subscribe(
      result => {
        for (let j = 0; j < result.length; j++) {
          this.networkDataService.isFriend(result[j].Id).subscribe(
            res => {
              result[j].IsFriend = res;
              this.loaderService.hide();
            },
            error => {
              this.loaderService.hide();
              this.toastrService.error(error.error, 'Error');
            }
          );
        }

        this.found = result.length;
        let current = 0;
        let fives = 0;
        for (let i = 0; i < result.length; i++) {
          if (current == 0) {
            this.searchFriends[fives] = [];
          }
          this.searchFriends[fives][current] = result[i];
          current++;
          if (current == 5) {
            current = 0;
            fives++;
          }
        }
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  onSubmit(event) {
    if (event.key == "Enter") {
      this.search();
    }
  }

  addFriend(friend: UserSmall) {
    this.networkDataService.AddFriend(friend.Id).subscribe(
      result => {
        friend.IsFriend = true;
        this.loaderService.hide();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  acceptFriend(friend: UserSmall) {
    this.networkDataService.AcceptFriend(friend.Id).subscribe(
      result => {
        friend.IsFriend = true;
        this.pending = 0;
        this.allFriends[0].unshift(friend);
        this.loaderService.hide();
        this.budgiesService.getBudgies();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }

  rejectFriend(friend: UserSmall) {
    this.networkDataService.RejectFriend(friend.Id).subscribe(
      result => {
        friend.IsFriend = false;
        this.networkDataService.getPendingFriends().subscribe(
          result => {
            this.pending = result.length;
            let current = 0;
            let fives = 0;
            for (let i = 0; i < result.length; i++) {
              if (current == 0) {
                this.pendingFriends[fives] = [];
              }
              this.pendingFriends[fives][current] = result[i];
              current++;
              if (current == 5) {
                current = 0;
                fives++;
              }
            }
            this.loaderService.hide();
          },
          error => {
            this.loaderService.hide();
            this.toastrService.error(error.error, 'Error');
          }
        );
        this.loaderService.hide();
        this.budgiesService.getBudgies();
      },
      error => {
        this.loaderService.hide();
        this.toastrService.error(error.error, 'Error');
      }
    );
  }


}
