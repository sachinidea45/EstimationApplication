import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './services/authentication.service';
import { UserResponse } from './models/UserResponse';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'Estimation Application';

  currentUser: UserResponse;
  currentUserName: string;
  currentUserRole: string;

  constructor(
     private router: Router,
     private authenticationService: AuthenticationService ) {
       this.authenticationService.currentUser.subscribe(x => {
         this.currentUser = x;
         if(x != null) {
           this.currentUserName = x.customer.userName;
           this.currentUserRole = x.customer.userCategories[0];
        }
      });
    }

    logout() {
      this.authenticationService.logout();
      this.router.navigate(['/login']);
    }
}
