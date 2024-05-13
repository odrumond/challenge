import { describe, it, expect, vi } from 'vitest'
import { mount } from '@vue/test-utils';
import AgentsTable from './AgentsTable.vue';

describe('AgentsTable', () => {
  const doMock = () => {
    const responseData = [
      { id: 1, name: 'Agent 1', state: 'ON_CALL', timeStampUtc: '2024-05-10T12:00:00Z' },
      { id: 2, name: 'Agent 2', state: 'ON_LUNCH', timeStampUtc: '2024-05-10T12:15:00Z' },
    ];
    
    vi.stubGlobal("$fetch", async () => Promise.resolve(responseData));
    
  }
  
  it('renders the table with correct data', async () => {
    doMock();

    const wrapper = mount(AgentsTable);  
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();

    expect(wrapper.find('table').exists()).toBe(true);
    expect(wrapper.findAll('tr')).toHaveLength(3); // Header row + 2 data rows
    expect(wrapper.findAll('tr').at(1)?.text()).toContain('Agent 1');
    expect(wrapper.findAll('tr').at(1)?.text()).toContain('ON_CALL');
    expect(wrapper.findAll('tr').at(1)?.text()).toContain('Friday, May 10th, 2024 - 13:00:00');
    expect(wrapper.findAll('tr').at(2)?.text()).toContain('Agent 2');
    expect(wrapper.findAll('tr').at(2)?.text()).toContain('ON_LUNCH');
    expect(wrapper.findAll('tr').at(2)?.text()).toContain('Friday, May 10th, 2024 - 13:15:00');
  });

  it('should fire sort when TimeStamp column is clicked', async () => {
    const wrapper = mount(AgentsTable);  
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();

    const sortSpy = vi.fn();
    wrapper.vm.sort = sortSpy;

    const rows = wrapper.findAll('tr').at(0);
    rows?.findAll('th').at(2)?.trigger('click');

    expect(sortSpy).toHaveBeenCalled();
  });

  it('should fire sort when State column is clicked', async () => {
    const wrapper = mount(AgentsTable);  
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();
    await wrapper.vm.$nextTick();

    const sortSpy = vi.fn();
    wrapper.vm.sort = sortSpy;

    const rows = wrapper.findAll('tr').at(0);
    rows?.findAll('th').at(1)?.trigger('click');

    expect(sortSpy).toHaveBeenCalled();
  })

});
