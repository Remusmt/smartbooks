import { UnitofMeasureService } from 'src/app/services/unitof-measure.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { UnitofMeasure } from 'src/app/models/unitof-measure';
import { NotificationService } from 'src/app/shared/services/notification.service';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-unitof-measure',
  templateUrl: './unitof-measure.component.html',
  styleUrls: ['./unitof-measure.component.css']
})
export class UnitofMeasureComponent implements OnInit, OnDestroy {

  saving = false;
  selected: UnitofMeasure;
  private subscriptions: Subscription = new Subscription();

  uomTypes = [
    {value: 0, description: 'Count'},
    {value: 1, description: 'Weight'},
    {value: 2, description: 'Length'},
    {value: 3, description: 'Area'},
    {value: 4, description: 'Volume'},
    {value: 5, description: 'Time'}
  ];

  constructor(
    public service: UnitofMeasureService,
    private notificationService: NotificationService,
    private dialogRef: MatDialogRef<UnitofMeasureComponent>) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.service.selected
        .subscribe(
          res => {
            this.selected = res;
          }
      ));
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  onSubmit(): void {
    if (this.service.form.valid) {
      this.saving = true;
      if (this.service.form.get('id').value !== 0) {
        this.subscriptions.add(
          this.service.onEdit(this.service.form.value)
          .subscribe(
            res => {
              this.notificationService.success(' Updated successfully');
              this.saving = false;
              this.onClose();
            },
            err => {
              this.notificationService.warn(err.error);
              this.saving = false;
            }
        ));
      } else {
        this.subscriptions.add(
          this.service.onCreate(this.service.form.value)
            .subscribe(
              res => {
                this.notificationService.success(' Saved successfully');
                this.saving = false;
                this.onClose();
              },
              err => {
                console.log(err);
                this.notificationService.warn(err);
                this.saving = false;
              }
        ));
      }
    }
  }

  onClose(): void {
    this.service.uomConversionForm.reset();
    this.service.initializeConversionFormGroup(null);
    this.dialogRef.close();
  }

}
