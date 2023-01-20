import { Country } from './../models/country';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ConstantsService } from '../shared/services/constants.service';
import { BehaviorSubject } from 'rxjs';
import { Currency } from '../models/currency';
import { Bank } from '../models/bank';

@Injectable({
  providedIn: 'root'
})
export class CompanyService{

  controller = 'Company';
  countriesWereFetched = false;
  private countryDataSourceSubject = new BehaviorSubject<Country[]>([]);
  countryDataSource = this.countryDataSourceSubject.asObservable();

  currenciesWereFetched = false;
  private currencyDataSourceSubject = new BehaviorSubject<Currency[]>([]);
  currencyDataSource = this.currencyDataSourceSubject.asObservable();

  banksWereFetched = false;
  private bankDataSourceSubject = new BehaviorSubject<Bank[]>([]);
  bankDataSource = this.bankDataSourceSubject.asObservable();

  constructor(
    private http: HttpClient,
    private constants: ConstantsService) {
      this.onGetCountries();
      this.onGetCurrencies();
    }

  onGetCountries(): void {
    if (!this.countriesWereFetched){
      this.http.get<Country[]>(`${this.constants.baseUrl}${this.controller}/GetCountries`)
      .subscribe(
        res => {
          this.countryDataSourceSubject.next(res);
          this.countriesWereFetched = true;
        }
      );
    }
  }

  onGetCurrencies(): void {
    if (!this.currenciesWereFetched){
      this.http.get<Currency[]>(`${this.constants.baseUrl}${this.controller}/GetCurrencies`)
      .subscribe(
        res => {
          this.currencyDataSourceSubject.next(res);
          this.currenciesWereFetched = true;
        }
      );
    }
  }

  onGetBanks(countryId: number): void {
    if (!this.banksWereFetched){
      this.http.get<Bank[]>(`${this.constants.baseUrl}${this.controller}/GetBanks/${countryId}`)
      .subscribe(
        res => {
          this.bankDataSourceSubject.next(res);
          this.banksWereFetched = true;
        }
      );
    }
  }

}
