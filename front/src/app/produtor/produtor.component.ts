import { Component, Input, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { environment as env } from 'src/environments/environment';

@Component({
  selector: 'app-produtor',
  templateUrl: './produtor.component.html',
  styleUrls: ['./produtor.component.scss'],
})
export class ProdutorComponent {
  inputText: string = '';

  generateRandomString() {
    const allowedCharacters =
      'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%&_';
    const charactersLength = Math.floor(Math.random() * 6) + 15;

    return Array.from({ length: charactersLength }, () => {
      const index = Math.floor(Math.random() * allowedCharacters.length);
      return allowedCharacters.charAt(index);
    }).join('');
  }

  onClickGenerateRandomString() {
    this.inputText = this.generateRandomString();
  }

  onSendButtonClick() {
    try {
      const apiUrl = env.apiUrl + '/contador/send-text';
      fetch(apiUrl, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ text: this.inputText }),
      }).then((response) => {
        if (response.ok) {
          alert('Texto enviado com sucesso!');
        } else {
          alert('Erro ao enviar o texto.');
        }
      });
    } catch (error) {
      alert('Error: ' + error);
    }
  }
}
