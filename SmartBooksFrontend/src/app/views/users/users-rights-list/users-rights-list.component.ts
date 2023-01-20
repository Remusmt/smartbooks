import { SelectionModel } from '@angular/cdk/collections';
import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { Subscription } from 'rxjs';
import { Claim, TreeUserRightNode, UserModel, UserRightGroup } from 'src/app/models/user-model';
import { UsersService } from 'src/app/services/user.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-users-rights-list',
  templateUrl: './users-rights-list.component.html',
  styleUrls: ['./users-rights-list.component.css']
})
export class UsersRightsListComponent implements OnInit, OnDestroy {

  user: UserModel;
  private subscriptions: Subscription = new Subscription();

  checklistSelection = new SelectionModel<TreeUserRightNode>(true/* multiple */);

  treeControl = new FlatTreeControl<TreeUserRightNode>(
    node => node.level, node => node.expandable);

  private transformer = (node: UserRightGroup, level: number) => {
    return {
      expandable: !!node.rights && node.rights.length > 0,
      description: node.description,
      claimType: node.claimType,
      granted: node.granted,
      level,
    };
  }

  // tslint:disable-next-line: member-ordering
  treeFlattener = new MatTreeFlattener(
    this.transformer, node => node.level, node => node.expandable, node => node.rights);

  // tslint:disable-next-line: member-ordering
  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(
    public usersService: UsersService,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.subscriptions.add(
      this.usersService.selectedRecord.subscribe(
        (usr: UserModel) => {
          this.user = usr;
          this.dataSource.data = this.user.userRights;

          this.dataSource._flattenedData.value.forEach(e => {
            if (e.granted) {
              this.checklistSelection.select(e);
            }
          });

          this.treeControl.expandAll();
        }
      )
    );
  }


  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  hasChild = (_: number, node: TreeUserRightNode) => node.expandable;

    /** Toggle a leaf to-do item selection. Check all the parents to see if they changed */
    todoLeafItemSelectionToggle(node: TreeUserRightNode): void {
      this.checklistSelection.toggle(node);
      this.checkAllParentsSelection(node);
    }

      /* Checks all the parents when a leaf node is selected/unselected */
  checkAllParentsSelection(node: TreeUserRightNode): void {
    let parent: TreeUserRightNode | null = this.getParentNode(node);
    while (parent) {
      this.checkRootNodeSelection(parent);
      parent = this.getParentNode(parent);
    }
  }

  getLevel = (node: TreeUserRightNode) => node.level;

  /** Check root node checked state and change it accordingly */
  checkRootNodeSelection(node: TreeUserRightNode): void {
    const nodeSelected = this.checklistSelection.isSelected(node);
    const descendants = this.treeControl.getDescendants(node);
    const descAllSelected = descendants.length > 0 && descendants.every(child => {
      return this.checklistSelection.isSelected(child);
    });
    if (nodeSelected && !descAllSelected) {
      node.granted = false;
      this.checklistSelection.deselect(node);
    } else if (!nodeSelected && descAllSelected) {
      node.granted = true;
      this.checklistSelection.select(node);
    }
  }

    /* Get the parent node of a node */
  getParentNode(node: TreeUserRightNode): TreeUserRightNode | null {
    const currentLevel = this.getLevel(node);

    if (currentLevel < 1) {
      return null;
    }

    const startIndex = this.treeControl.dataNodes.indexOf(node) - 1;

    for (let i = startIndex; i >= 0; i--) {
      const currentNode = this.treeControl.dataNodes[i];

      if (this.getLevel(currentNode) < currentLevel) {
        return currentNode;
      }
    }
    return null;
  }

    /** Whether all the descendants of the node are selected. */
    descendantsAllSelected(node: TreeUserRightNode): boolean {
      const descendants = this.treeControl.getDescendants(node);
      const descAllSelected = descendants.length > 0 && descendants.every(child => {
        return this.checklistSelection.isSelected(child);
      });
      return descAllSelected;
    }

      /** Whether part of the descendants are selected */
  descendantsPartiallySelected(node: TreeUserRightNode): boolean {
    const descendants = this.treeControl.getDescendants(node);
    const result = descendants.some(child => this.checklistSelection.isSelected(child));
    return result && !this.descendantsAllSelected(node);
  }

  todoItemSelectionToggle(node: TreeUserRightNode): void {
    node.granted = ! node.granted;
    this.checklistSelection.toggle(node);
    const descendants = this.treeControl.getDescendants(node);
    this.checklistSelection.isSelected(node)
      ? this.checklistSelection.select(...descendants)
      : this.checklistSelection.deselect(...descendants);

    // Force update for the parent
    descendants.forEach(child => this.checklistSelection.isSelected(child));
    this.checkAllParentsSelection(node);
  }

  saveRights(): void {
    const claims: Claim[] = [];
    this.dataSource._flattenedData.value.forEach(e => {
      if (!e.expandable) {
        claims.push({
          type: e.claimType,
          value: this.checklistSelection.isSelected(e) ? 'true' : 'false'
        });
      }
    });
    this.subscriptions.add(
      this.usersService.onSaveUserRights(this.user.email, claims)
        .subscribe(
          _ => {
            this.notificationService.success(' Saved successfully');
          },
          err => {
            this.notificationService.warn(err);
          }
    ));
  }

}
