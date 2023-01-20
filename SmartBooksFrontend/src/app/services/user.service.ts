import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Claim, UserModel } from '../models/user-model';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  private dataSourceSubject = new BehaviorSubject<UserModel[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  private selectedRecordSubject: BehaviorSubject<UserModel>;
  public selectedRecord: Observable<UserModel>;

  public recordsFetched: boolean;

  editUserForm: FormGroup = new FormGroup({
    email:  new FormControl('', Validators.required),
    fullName:  new FormControl('', Validators.required),
    phoneNumber: new FormControl('')
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) {
      const user = {
        email: '',
        fullName: '',
        phoneNumber: '',
        userRights: []
      };
      this.selectedRecordSubject = new BehaviorSubject<UserModel>(user);
      this.selectedRecord = this.selectedRecordSubject.asObservable();
    }

    populateEditUserForm(user: UserModel): void {
      this.editUserForm.setValue({
        email: user.email,
        fullName: user.fullName,
        phoneNumber: user.phoneNumber
      });
    }

    public setSelectedRecord(user: UserModel): void {
      this.selectedRecordSubject.next(user);
    }

    onGet(forceReload = false): void {
      if (!this.recordsFetched && !forceReload){
        this.http.get<UserModel[]>(`${this.constants.baseUrl}accounts/GetUsers`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.recordsFetched = true;
          }
        );
      }
    }

    onCreate(model: UserModel): Observable<UserModel> {
      return this.http.post<UserModel>(`${this.constants.baseUrl}accounts/CreateUser`, model)
      .pipe(
        map(res => {
          this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
          this.dataSourceSubject.next(this.dataSourceSubject.value);
          return res;
        })
      );
    }

    onEdit(model: UserModel): Observable<UserModel> {
      return this.http.put<UserModel>(`${this.constants.baseUrl}accounts/EditUser`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.email === res.email).fullName =  res.fullName;
            this.dataSourceSubject.value.find(e => e.email === res.email).phoneNumber =  res.phoneNumber;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onSaveUserRights(userName: string, claims: Claim[]): Observable<UserModel> {
      return this.http.post<UserModel>(`${this.constants.baseUrl}accounts/SaveRights`, {username: userName, claims})
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.email === res.email).userRights =  res.userRights;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

}
