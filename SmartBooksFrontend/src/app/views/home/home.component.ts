import { CurrentUser, FlatTreeMenuNode, MenuNode } from './../../shared/models/current-user';
import { Component, OnInit } from '@angular/core';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { FlatTreeControl } from '@angular/cdk/tree';
import { MatTreeFlattener, MatTreeFlatDataSource } from '@angular/material/tree';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  isHandset$: Observable<boolean> = this.breakpointObserver.observe([
    Breakpoints.XSmall,
    Breakpoints.Small
  ])
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

    currentUserValue: CurrentUser;
    isMobile: boolean;
    private currentLink = 0;

    treeControl = new FlatTreeControl<FlatTreeMenuNode>(
      node => node.level, node => node.expandable);

    private transformer = (node: MenuNode, level: number) => {
      return {
        expandable: !!node.links && node.links.length > 0,
        description: node.description,
        linkUrl: node.linkUrl,
        icon: node.icon,
        imageIcon: node.imageIcon,
        level,
      };
    }

  // tslint:disable-next-line: member-ordering
  treeFlattener = new MatTreeFlattener(
    this.transformer, node => node.level, node => node.expandable, node => node.links);

  // tslint:disable-next-line: member-ordering
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);


  constructor(private breakpointObserver: BreakpointObserver,
              private authenticationService: AuthenticationService,
              private router: Router) {
    authenticationService.currentUser.subscribe(
      (res: CurrentUser) => {
        this.currentUserValue = res;
      }
    );
  }

  hasChild = (_: number, node: FlatTreeMenuNode) => node.expandable;

  ngOnInit(): void {
    this.isHandset$.subscribe(
      res => {
        this.isMobile = res;
      }
    );

    this.dataSource.data = this.currentUserValue.menus;

    if (this.router.url.endsWith('reports')) {
      this.router.navigate(['/app/reports']);
      this.currentLink = 2;
    } else if (this.router.url.endsWith('reconciliation')) {
      this.router.navigate(['/app/reconciliation']);
      this.currentLink = 1;
    } else {
      this.router.navigate(['/app']);
      this.currentLink = 0;
    }
  }

  logout(): void {
    this.authenticationService.logout();
  }

  changePassword(): void {
    // show modal form
  }

  toggleSideNav(drawer: MatSidenav): void {
    if (this.isMobile) {
      drawer.toggle();
    }
  }

  isCurrentLink(id: number): boolean {
    return this.currentLink === id;
  }

  reportLinkClicked(url: string): void {
    this.router.navigate([url]);
  }

  navLinkClicked(linkId: number): void {
    this.currentLink = linkId;
    switch (linkId) {
      case 0:
        this.router.navigate(['/app']);
        break;
      case 1:
        this.currentLink = linkId;
        this.router.navigate(['/app/reconciliation']);
        break;
      case 2:
        this.router.navigate(['/app/reports']);
        break;
    }

  }

}
