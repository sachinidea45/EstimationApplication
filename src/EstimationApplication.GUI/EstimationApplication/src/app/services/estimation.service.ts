import { Injectable } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { HttpClient } from "@angular/common/http";

import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";

import { environment } from "@environments/environment";
import { UserResponse } from "@app/models/UserResponse";
import { AuthenticationService } from "./authentication.service";

@Injectable({ providedIn: 'root' })
export class EstimationService {
    
    private currentUserSubject: BehaviorSubject<UserResponse>;
    public currentUser: Observable<UserResponse>;

    constructor(
        private http: HttpClient,
        private authenticationService: AuthenticationService) { 
        this.authenticationService.currentUser.subscribe();
        this.currentUserSubject = new BehaviorSubject<UserResponse>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    estimate(userFormGroup: FormGroup, buttonName: string) {
        if(buttonName == "Calculate"){
            const formData = new FormData();
            formData.append('goldpricepergram', userFormGroup.get('goldPricePerGram').value);
            formData.append('weightingram', userFormGroup.get('weightInGrams').value);
            return this.http.post<any>(`${environment.apiUrl}/Estimation/getEstimate`, formData)
            .pipe(map(estimation => {
                return estimation;
            }));
        }
        else{
            const formData = new FormData();
            formData.append('Estimation.GoldPricePerGram', userFormGroup.get('goldPricePerGram').value);
            formData.append('Estimation.WeightInGram', userFormGroup.get('weightInGrams').value);
            switch(buttonName){
                case "PrintToScreen":
                    formData.append('PrintType','0');
                    break;
                case "PrintToFile":
                    formData.append('PrintType','1');
                    break;
                case "PrintToPaper":
                    formData.append('PrintType','2');
                    break;
            }
            return this.http.post<any>(`${environment.apiUrl}/Estimation/getPrintEstimate`, formData)
            .pipe(map(estimation => {
                return estimation;
            }));
        }
    }
}