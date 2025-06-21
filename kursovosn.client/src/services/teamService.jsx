// teamService.js
import axios from 'axios';

const API_URL = 'http://localhost:5082/api/teams/';  // Укажите свой API URL

export const createTeam = async (teamData) => {
    const response = await axios.post(API_URL, teamData);
    return response.data;
};

export const getTeams = async () => {
    const response = await axios.get(API_URL);
    return response.data;
};
