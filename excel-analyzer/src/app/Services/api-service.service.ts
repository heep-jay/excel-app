import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface Results {
  divisibleBy2: string[];
  divisibleBy7: string[];
  divisibleBy3: string[];
  mean: number;
  median: number;
  shortestTo35: number[];
  shortestTo65: number[];
  sumOfDoubleDigit: number;
  sumOfEven: number;
  sumOfSingleDigit: number;
  sumofOdd: number;
}

const httpOption = {
  headers: new HttpHeaders({
    'Cache-Control': 'max-age=7200',
  }),
};

@Injectable({
  providedIn: 'root',
})
export class ApiServiceService {
  private apiUrl = 'http://localhost:5007';

  constructor(private http: HttpClient) {}

  getResults(formData: any): Observable<Results> {
    try {
      return this.http
        .post(`${this.apiUrl}/api/upload`, formData, httpOption)
        .pipe(
          map((res: any) => {
            return res;
          })
        );
    } catch (error: any) {
      return error.message;
    }
  }
}
