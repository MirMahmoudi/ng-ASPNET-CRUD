import { Component, OnInit } from '@angular/core';
import { CardsService } from './service/cards.service';
import { Card } from './models/card.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'cards';
  public cards: Card[] = [];
  public card: Card = new Card();

  constructor(private cardsService: CardsService){}

  ngOnInit(): void {
    this.cardsService.getAllCards()
      .subscribe( (result: Card[]) => this.cards = result );
  }

  public onSubmit(): void {
    if(this.card.id === ''){
      this.cardsService.addCard(this.card)
        .subscribe( (result: Card) => {
          this.cards.push(result);
        });
    } else {
      this.cardsService.updateCard(this.card)
        .subscribe();
    }
    this.card = new Card();
  }

  public onPopulate(card: Card){
    this.card = new Card();
    this.card = card;
  }

  public onDelete(id: string) {
    this.cardsService.deleteCard(id)
      .subscribe(() => this.cards = this.cards.filter(card => card.id !== id));
  }
}
