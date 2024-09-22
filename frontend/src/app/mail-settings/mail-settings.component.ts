import {Component, OnInit} from '@angular/core';
import {OrganizationAdminResponse, OrganizationDto, OrganizationService} from "../api";
import {firstValueFrom} from "rxjs";
import {FormArray, FormBuilder, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-mail-settings',
  templateUrl: './mail-settings.component.html',
  styleUrls: ['./mail-settings.component.scss']
})
export class MailSettingsComponent implements OnInit {

  public organizations: OrganizationAdminResponse[] = [];
  public form!: FormGroup;
  public isLoading = true;

  constructor(private readonly organizationService: OrganizationService,
              private readonly formBuilder: FormBuilder) {

  }

  async ngOnInit(): Promise<void> {
    this.isLoading = true;
    this.form = this.formBuilder.group({
      organizations: this.formBuilder.array([])
    });
    this.organizations = [];
    this.organizations = (await firstValueFrom(this.organizationService.getOrganizationAdminEndpoint())).organizations;
    this.populateForm();
    this.isLoading = false;
  }

  populateForm(): void {
    const orgArray = this.form.get('organizations') as FormArray;
    this.organizations?.forEach(org => {
      orgArray.push(this.formBuilder.group({
        name: [org.name],
        id: [org.id],
        isAdmin: [org.isAdmin]
      }));
    });
  }

  save() {
    console.log(this.form.value);

    const allAdminOrgIds = this.organizationsFormArray.controls
      .filter((control) => control.get('isAdmin')?.value)
      .map((control) => control.get('id')?.value);

    console.log(allAdminOrgIds);

    this.organizationService.setOrganizationAdminEndpoint({
      adminOrganizationIds: allAdminOrgIds
    })
      .subscribe(() => {
        this.ngOnInit();
      });
  }

  get organizationsFormArray(): FormArray {
    return this.form.get('organizations') as FormArray;
  }

}
