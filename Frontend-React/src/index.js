import React, { useState, useEffect } from 'react';
import axios from 'axios';

const TodoList = () => {
  const [todoItems, setTodoItems] = useState([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchTodoItems = async () => {
      try {
        const response = await axios.get('/api/TodoItems');
        setTodoItems(response.data);
      } catch (err) {
        setError(err.response ? err.response.data : 'Error fetching todo items');
      }
    };
  
    fetchTodoItems();
  }, []);


  const handleMarkComplete = async (id) => {
    try {
      await axios.put(`/api/TodoItems/${id}`, { isCompleted: true });
    } catch (err) {
      setError(err.response ? err.response.data : 'Error marking an item as complete');
    }
  };

  return (
    <div>
      {error && <p style={{ color: 'blue' }}>{error}</p>}
      <ul>
        {todoItems.map((item) => (
          <li key={item.id}>
            {item.description} - {!item.isCompleted && <button onClick={() => handleMarkComplete(item.id)}>Mark Complete</button>}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TodoList;
