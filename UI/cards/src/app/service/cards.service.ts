import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Card } from '../models/card.model';

@Injectable({
  providedIn: 'root'
})
export class CardsService {
  private baseUrl: string = 'https://localhost:7213';

  constructor(private http: HttpClient) { }

  public getAllCards = (): Observable<Card[]> =>
    this.http.get<Card[]>(`${this.baseUrl}/api/Cards`, this.getOption());

  public addCard = (card: Card): Observable<Card> => {
    card.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<Card>(`${this.baseUrl}/api/Cards`, card, this.getOption());
  }

  public updateCard = (card: Card): Observable<Card> =>
    this.http.put<Card>(`${this.baseUrl}/api/Cards/${card.id}`, card, this.getOption())

  public deleteCard = (id: string): Observable<Card> =>
    this.http.delete<Card>(`${this.baseUrl}/api/Cards/${id}`, this.getOption())

  private getOption = (): {headers: HttpHeaders} =>
    ({headers: new HttpHeaders({'Content-Type': 'application/json'})});
}
