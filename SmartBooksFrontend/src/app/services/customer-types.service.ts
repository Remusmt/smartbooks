import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Category } from '../models/category';
import { ConstantsService } from '../shared/services/constants.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerTypesService {

  private dataSourceSubject = new BehaviorSubject<Category[]>([]);
  dataSource = this.dataSourceSubject.asObservable();

  public wasFetched: boolean;

  form: FormGroup = new FormGroup({
    id: new FormControl(null),
    code:  new FormControl(''),
    description:  new FormControl('', Validators.required),
  });

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) { }

    initializeFormGroup(): void {
      this.form.setValue({
        id: 0,
        code:  '',
        description:  ''
      });
    }

    populateFormGroup(model: Category): void {
      this.form.setValue({
        id: model.id,
        code: model.code,
        description: model.description
      });
    }

    onGet(forceReload = false): void {
      if (!this.wasFetched && !forceReload){
        this.http.get<Category[]>(`${this.constants.baseUrl}CustomerTypes`)
        .subscribe(
          res => {
            this.dataSourceSubject.next(res);
            this.wasFetched = true;
          }
        );
      }
    }

    onCreate(model: Category): Observable<Category> {
      return this.http.post<Category>(`${this.constants.baseUrl}CustomerTypes`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.next(this.dataSourceSubject.getValue().concat([res]));
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onEdit(model: Category): Observable<Category> {
      return this.http.put<Category>(`${this.constants.baseUrl}CustomerTypes`, model)
        .pipe(
          map(res => {
            this.dataSourceSubject.value.find(e => e.id === res.id).code =  res.code;
            this.dataSourceSubject.value.find(e => e.id === res.id).description =  res.description;
            this.dataSourceSubject.next(this.dataSourceSubject.value);
            return res;
          })
        );
    }

    onDelete(id: number): Observable<number> {
      return this.http.delete<number>(`${this.constants.baseUrl}CustomerTypes/${id}`)
      .pipe(
        map(res => {
          const index = this.dataSourceSubject.value.findIndex(e => e.id === res);
          if (index > -1) {
            this.dataSourceSubject.value.splice(index, 1);
            this.dataSourceSubject.next(this.dataSourceSubject.value);
          }
          return res;
        })
      );
    }
}
