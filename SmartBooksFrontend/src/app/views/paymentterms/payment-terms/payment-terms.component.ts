import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { PaymentTermsService } from 'src/app/services/payment-terms.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-payment-terms',
  templateUrl: './payment-terms.component.html',
  styleUrls: ['./payment-terms.component.css']
})
export class PaymentTermsComponent implements OnInit, OnDestroy {

  saving = false;
  dateDriven = false;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public service: PaymentTermsService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<PaymentTermsComponent>) { }

  ngOnInit(): void {
    this.dateDriven = this.service.form.get('dateDriven').value;
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  termsBaseChanged(): void {
    this.dateDriven = !this.dateDriven;
    this.service.form.get('dateDriven').setValue(this.dateDriven);
  }

  onSubmit(): void {
    if (this.service.form.valid) {
      this.saving = true;
      if (this.service.form.get('id').value !== 0) {
        this.subscriptions.add(
          this.service.onEdit(this.service.form.value)
          .subscribe(
            _ => {
              this.notificationService.success(' Updated successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              console.log(err);
              this.notificationService.warn(err);
              this.saving = false;
            }
        ));
      } else {
        this.subscriptions.add(
          this.service.onCreate(this.service.form.value)
            .subscribe(
              _ => {
                this.notificationService.success(' Saved successfully');
                this.saving = false;
                this.onClose();
              },
              err => {
                this.notificationService.warn(err);
                this.saving = false;
              }
        ));
      }
    }
  }

  onClose(): void {
    this.service.form.reset();
    this.dialogRef.close();
  }

}
