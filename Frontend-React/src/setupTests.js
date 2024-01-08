import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import axios from 'axios';
import TodoList from './TodoList';

jest.mock('axios');

describe('TodoList', () => {
  test('fetches and displays todo items', async () => {
    const mockData = [
      { id: '1', description: 'Task 1', isCompleted: false },
      { id: '2', description: 'Task 2', isCompleted: true },
    ];

    axios.get.mockResolvedValueOnce({ data: mockData });

    render(<TodoList />);

    await waitFor(() => {
      expect(screen.getByText('Task 1')).toBeInTheDocument();
      expect(screen.getByText('Task 2')).toBeInTheDocument();
    });
  });

  test('handles error during fetching todo items', async () => {
    axios.get.mockRejectedValueOnce(new Error('API Error'));

    render(<TodoList />);

    await waitFor(() => {
      expect(screen.getByText('Error fetching todo items')).toBeInTheDocument();
    });
  });

  test('handles error during marking an item as complete', async () => {
    const mockData = [{ id: '1', description: 'Task 1', isCompleted: false }];
    axios.get.mockResolvedValueOnce({ data: mockData });
    axios.put.mockRejectedValueOnce(new Error('API Error'));
  
    render(<TodoList />);
  
    await waitFor(() => {
      expect(screen.getByText('Task 1')).toBeInTheDocument();
    });
  
    fireEvent.click(screen.getByText('Mark Complete'));
  
    await waitFor(() => {
      expect(screen.getByText('Error marking an item as complete')).toBeInTheDocument();
    });
  });  
});
