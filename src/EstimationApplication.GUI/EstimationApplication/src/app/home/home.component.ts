import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { BehaviorSubject } from 'rxjs';
import { first } from 'rxjs/operators';

import { UserResponse } from '@app/models/UserResponse';
import { EstimationResponse } from '@app/models/EstimationResponse';
import { PrintResponse } from '@app/models/PrintResponse';

import { AuthenticationService } from '@app/services/authentication.service';
import { EstimationService} from '@app/services/estimation.service'
import { Router } from '@angular/router';

@Component({
    selector: 'app-estimate',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
  })
export class HomeComponent {
    currentUser : UserResponse;

    estimationForm: FormGroup;
    loading = false;
    estimationSubmitted = false;
    error = '';

    private currentEstimationSubject: BehaviorSubject<EstimationResponse>;
    private currentPrintSubject: BehaviorSubject<PrintResponse>;

    estimationResult: EstimationResponse;
    printResult: PrintResponse;

    constructor(
        private router: Router,
        private formBuilder: FormBuilder,
        private estimationService: EstimationService,
        private authenticationService: AuthenticationService){
            this.currentUser = authenticationService.currentUserValue;

            this.currentEstimationSubject =  new BehaviorSubject<EstimationResponse>(null);
            this.currentPrintSubject =  new BehaviorSubject<PrintResponse>(null);
        }

    ngOnInit() {
        this.estimationForm = this.formBuilder.group({
            goldPricePerGram: ['', Validators.required],
            weightInGrams: ['', Validators.required]
        });
    }

    get f() {
        return this.estimationForm.controls;
    }

    onEstimationSubmit(){
        this.estimationSubmitted = true;

        // stop here if form is invalid
        if (this.estimationForm.invalid) {
            return;
        }

        this.loading = true;
        var buttonName = document.activeElement.getAttribute("value");

        this.estimationService.estimate(this.estimationForm, buttonName)
            .pipe(first())
            .subscribe(
                data => {
                    this.loading = false;
                    if(buttonName == "Calculate"){
                        this.currentEstimationSubject.next(data.estimate);
                        this.estimationResult = this.currentEstimationSubject.value;
                    }
                    else{
                        if(data.result.print != null){
                            this.currentPrintSubject.next(data.result.print);
                            this.printResult = this.currentPrintSubject.value;
                            alert(this.printResult.printMessageOutput);
                            return;
                        }
                        alert(data.result.message);
                    }
                },
                error => {
                    this.error = error;
                    this.loading = false;
                }
            );
    }
    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
      }
}