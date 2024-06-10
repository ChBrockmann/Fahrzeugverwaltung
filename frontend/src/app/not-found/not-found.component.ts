import {Component, OnInit} from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent implements OnInit{

  constructor(private readonly router: Router) {
  }

  ngOnInit(): void {
    setTimeout(() => {
      this.router.navigate(["/"]).then(r => console.log("Navigated to /"));
    }, 5000);
  }

  public quickActions = [
    {
      title: 'Startseite / Kalender',
      description: 'Zurück zur Startseite und zum Kalender',
      icon: 'calendar_today',
      link: '/calendar'
    },
    {
      title: 'Login',
      description: 'Zum Login, wenn Sie sich neu anmelden möchten',
      icon: 'login',
      link: '/login'
    },
    {
      title: 'Go to Accept Invitation',
      icon: 'how_to_reg',
      link: '/accept-invitation'
    }
  ]

}
