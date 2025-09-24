import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardVexpensesBimmerService {
  constructor() {}

  obterDashboardData(): Observable<any> {
    return of();
  }
}
