import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsumidorComponent } from './consumidor.component';

describe('ConsumidorComponent', () => {
  let component: ConsumidorComponent;
  let fixture: ComponentFixture<ConsumidorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ConsumidorComponent]
    });
    fixture = TestBed.createComponent(ConsumidorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
