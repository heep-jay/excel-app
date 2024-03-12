import { Component } from '@angular/core';
import { ApiServiceService } from 'src/app/Services/api-service.service';

export interface Results {
  divisibleBy2: string[];
  divisibleBy7: string[];
  divisibleBy3: string[];
  mean: number;
  median: number;
  shortestTo35: number[];
  shortestTo65: number[];
  sumOfDoubleDigit: number;
  sumOfEven: number;
  sumOfSingleDigit: number;
  sumofOdd: number;
}

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
})
export class ContentComponent {
  selectedFile: File | null = null;
  divisibleBy2: string[] = [];
  divisibleBy3: string[] = [];
  divisibleBy7: string[] = [];
  mean: number = 0;
  median: number = 0;
  shortestTo35: number[] = [];
  shortestTo65: number[] = [];
  sumOfDoubleDigit: number = 0;
  sumOfEven: number = 0;
  sumOfSingleDigit: number = 0;
  sumofOdd: number = 0;
  file: any;
  name?: string = 'No File selected';
  lastItems: string[] = [];
  nonLastItems: string[] = [];
  lastItems2: string[] = [];
  nonLastItems2: string[] = [];
  lastItems3: string[] = [];
  nonLastItems3: string[] = [];

  constructor(private api: ApiServiceService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
    console.log(this.selectedFile);
    this.name = this.selectedFile?.name;
  }

  reset() {
    window.location.reload();
  }

  uploadFile(): void {
    if (!this.selectedFile) return alert('No File Selected');
    if (this.selectedFile && !this.selectedFile.name.endsWith('.xlsx')) {
      alert('Inavlid File');
    }
    console.log(this.selectedFile);
    let formData: FormData = new FormData();
    formData.append('file', this.selectedFile, this.selectedFile.name);

    this.api.getResults(formData).subscribe((data: Results) => {
      this.divisibleBy2 = data.divisibleBy2;
      this.divisibleBy3 = data.divisibleBy3;
      this.divisibleBy7 = data.divisibleBy7;
      this.mean = data.mean;
      this.median = data.median;
      this.shortestTo35 = data.shortestTo35;
      this.shortestTo65 = data.shortestTo65;
      this.sumOfEven = data.sumOfEven;
      this.sumofOdd = data.sumofOdd;
      this.sumOfSingleDigit = data.sumOfSingleDigit;
      this.sumOfDoubleDigit = data.sumOfDoubleDigit;
      this.getLastThreeItems(this.divisibleBy2, -2);
      this.getNonRedItems(this.divisibleBy2, -2);
      this.getFirstThreeItems2(this.divisibleBy7, 4);
      this.getNonRedItems2(this.divisibleBy7, 4);
      this.getLastThreeItems3(this.divisibleBy3, -3);
      this.getNonRedItems3(this.divisibleBy3, -3);
    });
  }
  onSubmit(e: any) {
    e.preventDefault();
    this.uploadFile();
  }

  getLastThreeItems(array: string[], text: number): string[] {
    this.lastItems = array.slice(text);
    return array.slice(text); // Get the last three items
  }

  getNonRedItems(array: string[], text: number): string[] {
    this.nonLastItems = array.slice(0, text);
    return array.slice(0, text); // Get items excluding the last three
  }
  getFirstThreeItems2(array: string[], text: number): string[] {
    this.lastItems2 = array.slice(0, text);
    return array.slice(text); // Get the last three items
  }

  getNonRedItems2(array: string[], text: number): string[] {
    this.nonLastItems2 = array.slice(text);
    return array.slice(text); // Get items excluding the last three
  }

  getLastThreeItems3(array: string[], text: number): string[] {
    this.lastItems3 = array.slice(text);
    return array.slice(text); // Get the last three items
  }

  getNonRedItems3(array: string[], text: number): string[] {
    this.nonLastItems3 = array.slice(0, text);
    return array.slice(0, text); // Get items excluding the last three
  }
}
