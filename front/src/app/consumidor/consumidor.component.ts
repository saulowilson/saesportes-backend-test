import { Component, OnInit } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment as env } from 'src/environments/environment';

@Component({
  selector: 'app-consumidor',
  templateUrl: './consumidor.component.html',
  styleUrls: ['./consumidor.component.scss'],
})
export class ConsumidorComponent implements OnInit {
  private hubConnection!: HubConnection;
  messages: Message[] = [];
  serverConnected: boolean = false;
  ngOnInit(): void {
    const apiUrl = env.apiUrl + '/get-messages';
    let builder = new HubConnectionBuilder();
    this.hubConnection = builder.withUrl(apiUrl).build();

    this.hubConnection
      .start()
      .then(() => {
        this.serverConnected = true;
        console.log('Conectado com o servidor!');
      })
      .catch((err) => {
        this.serverConnected = false;
        console.log('Erro ao conectar: ' + err);
      });

    this.hubConnection.on('message', (data) => {
      this.messages.push(JSON.parse(data));
    });
  }
}

type Message = {
  Text: string;
  IsValid: boolean;
};
