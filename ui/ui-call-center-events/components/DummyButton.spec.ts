import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils';
import DummyButton from './DummyButton.vue';

describe('DummyButton', () => {
  it('emits a click event when clicked', async () => {
    // Arrange
    const wrapper = mount(DummyButton);
    
    // Act
    await wrapper.trigger('click');
    
    // Assert
    expect(wrapper.emitted().click).toBeTruthy();
  });

  it('displays custom text', () => {
    
    // Arrange
    const text = 'Click me!';
    
    // Act
    const wrapper = mount(DummyButton, {
      propsData: {
        text
      }
    });

    // Assert
    expect(wrapper.text()).toContain(text);
  });

});
