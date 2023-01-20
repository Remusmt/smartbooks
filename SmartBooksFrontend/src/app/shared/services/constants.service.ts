import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConstantsService {

  baseUrl = 'https://localhost:44383/api/'; // development

  constructor() {
    if (environment.production) {
      this.baseUrl = 'https://smartbooksbackend.witsoft.co.ke/api/';
    } else {
      this.baseUrl = 'https://localhost:44383/api/';
    }
  }
}
