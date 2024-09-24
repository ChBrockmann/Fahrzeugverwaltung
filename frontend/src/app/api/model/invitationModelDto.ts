/**
 * Fahrzeugverwaltung
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { InvitationModelDtoCreatedBy } from './invitationModelDtoCreatedBy';


export interface InvitationModelDto { 
    id?: string;
    token?: string;
    note?: string | null;
    createdBy?: InvitationModelDtoCreatedBy | null;
    createdAt?: Date;
    acceptedBy?: InvitationModelDtoCreatedBy | null;
    acceptedAt?: Date | null;
    roles?: Array<string>;
    expiresAt?: Date;
}

