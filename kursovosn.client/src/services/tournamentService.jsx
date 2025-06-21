// tournamentService.js
import axios from 'axios';
const API_URL = 'http://localhost:5082/api/tournament/';

export const tournamentService = {
    getAllTournaments: async () => {
        try {
            const response = await axios.get('http://localhost:5082/api/tournament/list');

            // Логируем ответ для отладки
            const data = response.data;
            console.log("Response data:", data);

            if (data.$values) {
                return data.$values;
            }

            return data;
        } catch (error) {
            console.error('Error fetching tournaments:', error);
            throw error;
        }
    },
};

export const getTournamentById = async (id) => {
    const response = await axios.get(`http://localhost:5082/api/tournament/${id}`);
    return response.data;
};

export const getParticipants = async (tournamentId) => {
    const response = await axios.get(`http://localhost:5082/api/tournament/${tournamentId}/participants`);
    return response.data;
};


export const createTournament = async (tournamentData) => {
    const token = sessionStorage.getItem("token");
    const response = await fetch('http://localhost:5082/api/tournament/create-tournament', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(tournamentData),
    });

    if (!response.ok) {
        const errorText = await response.text();
        console.error('Server response:', errorText); 
        throw new Error('Failed to create tournament');
    }

    return response.json();
};

