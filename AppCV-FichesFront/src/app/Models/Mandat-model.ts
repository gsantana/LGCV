import { TacheViewModel } from "./Tache-model";
import { TechnologieViewModel } from "./Technologie-model";

export class MandatViewModel {
  graphId: string;
  graphIdProjet: string;
  graphIdClient: string;
  graphIdFonction: string;
  graphIdSocieteDeConseil: string;

  nomClient: string;
  numeroMandat: Number;
  nomEntreprise: string;
  titreProjet: string;
  titreMandat: string;
  envergure: Number;
  efforts: Number;
  fonction: string;
  contexteProjet: string;
  porteeDesTravaux: string;


  debutProjet: Date;
  finProjet: Date;
  debutMandat: Date;
  finMandat: Date;

  nomReference: string;
  fonctionReference: string;
  telephoneReference: string;
  cellulaireReference: string;
  courrielReference: string;

  taches: Array<TacheViewModel>;
  technologies: Array<TechnologieViewModel>;

  constructor() {
    this.taches = new Array<TacheViewModel>();
    this.technologies = new Array<TechnologieViewModel>();
  }
}