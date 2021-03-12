import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root",
})
export class LocalStorageService {
  private currentLoggedInUserId = JSON.parse(this.get("user")).id;

  constructor() {}

  get(key: string) {
    return localStorage.getItem(key);
  }

  set(key: string, value: string) {
    localStorage.setItem(key, value);
  }

  remove(key: string) {
    localStorage.removeItem(key);
  }

  clear() {
    localStorage.clear();
    this.currentLoggedInUserId = null;
  }

  public getCurrentLoggedInUserId(): number {
    return this.currentLoggedInUserId;
  }
}
