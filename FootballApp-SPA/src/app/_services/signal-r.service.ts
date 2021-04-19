import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { HubConnection, HubConnectionBuilder } from "@aspnet/signalr";
import { Observable, Subject } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class SignalRService {
  private hubUrl: string = "http://localhost:5000/chatHub";
  private controllerUrl: string = "http://localhost:5000/api/chat";
  private readonly messageReceivedSource = new Subject<any>();
  public messageReceived$ = this.messageReceivedSource.asObservable();

  private connectionId: string = "";
  private _hubConnection: HubConnection;

  constructor(private http: HttpClient) {
    this.createConnection();
    this.startConnection();
    this.registerOnServerEvents();
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .build();
  }

  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        console.log("Hub connection started");
        this.getConnectionId();
      })
      .catch(() => {
        console.log("Error while establishing connection, retrying...");
        setTimeout(function () {
          this.startConnection();
        }, 5000);
      });
  }

  private getConnectionId(): void {
    this._hubConnection
      .invoke("getConnectionId")
      .then((conId) => {
        this.connectionId = conId;
      })
      .catch((err) => {
        console.log(err);
      });
  }

  private registerOnServerEvents(): void {
    this._hubConnection.on("ReceiveMessage", (data: any) => {
      // Here we receive message and than we emit it
      this.messageReceivedSource.next(data);
    });
  }

  public joinRoom(chatId: number) {
    this.http
      .post(
        this.controllerUrl + `/JoinRoom/${this.connectionId}/${chatId}`,
        null
      )
      .subscribe(
        (res) => {
          console.log(`Successfully joined room ${chatId}!`);
        },
        (err) => {
          console.error(err);
        }
      );
  }

  public sendMessage(content: string, chatId: number): Observable<any> {
    const data = {
      content,
      chatId,
    };
    return this.http.post(this.controllerUrl + `/SendMessage`, data);
  }
}
