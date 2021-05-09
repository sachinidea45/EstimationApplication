import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { UserResponse } from '@app/models/UserResponse';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<UserResponse>;
    public currentUser: Observable<UserResponse>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<UserResponse>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): UserResponse {
        return this.currentUserSubject.value;
    }

    login(userFormGroup: FormGroup) {
        const formData = new FormData();
        formData.append('username', userFormGroup.get('username').value);
        formData.append('password', userFormGroup.get('password').value);
        
        return this.http.post<any>(`${environment.apiUrl}/User/authentication`, formData)
        .pipe(map(user => {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }
        ));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}